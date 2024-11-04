function Test-ConfiguredSuggester {
    param(
        [string] $index
    )

    $suggesterUrl = ("{0}/{1}/suggest" -f $env:SITECORE_SOLR_CONNECTION_STRING, $index)
    $statusCode = 0
    try{
        $response = Invoke-WebRequest -Uri $suggesterUrl -UseBasicParsing
        $statusCode = $response.StatusCode
    } catch{
        $statusCode = $_.Exception.Response.StatusCode.Value__
    }
    return $statusCode -eq 200
}

function Add-SearchComponent {
    param(
        [string] $index
    )
    $url = ("{0}/{1}/config" -f $env:SITECORE_SOLR_CONNECTION_STRING, $index)

    $addComponentBody = @{
        "add-searchcomponent"= @{
            "name"="suggest"
            "class"="solr.SuggestComponent"
            "suggester"= @{
                "name"="sxaSuggester"
                "lookupImpl"="AnalyzingInfixLookupFactory"
                "field"="keywordtags_sm"
                "suggestAnalyzerFieldType"="text_general"
                "buildOnStartup"="true"
                "highlight"="false"
            }
        }
    } | ConvertTo-Json
    Invoke-RestMethod -uri $url -Method POST -ContentType "application/json" -Body $addComponentBody
}
function Add-RequestHandler {
    param(
        [string] $index
    )
    $url = ("{0}/{1}/config" -f $env:SITECORE_SOLR_CONNECTION_STRING, $index)
    
    $addHandlerBody = @{
        "add-requesthandler"=@{
            "name"="/suggest"
            "class"="solr.SearchHandler"
            "startup"="lazy"
            "defaults"=@{
                "suggest"="true"
                "suggest.count"="10"
                "suggest.dictionary"="sxaSuggester"
            }
            "components"=@("suggest")
        }
    } | ConvertTo-Json
    Invoke-RestMethod -uri $url -Method POST -ContentType "application/json" -Body $addHandlerBody
}

function Enable-Suggester {
    param(
        [string] $index
    )

    if (Test-ConfiguredSuggester -index $index) {
        write-host "sxaSuggester is already configured for $index"
    } else {
        write-host "configuring sxaSuggester for $index"
        Add-SearchComponent -index $index
        Add-RequestHandler -index $index
    }
}

.\Start.ps1 `
    -SitecoreSolrConnectionString $env:SITECORE_SOLR_CONNECTION_STRING `
    -SolrCorePrefix $env:SOLR_CORE_PREFIX_NAME `
    -SolrSitecoreConfigsetSuffixName $env:SOLR_SITECORE_CONFIGSET_SUFFIX_NAME `
    -SolrReplicationFactor $env:SOLR_REPLICATION_FACTOR `
    -SolrNumberOfShards $env:SOLR_NUMBER_OF_SHARDS `
    -SolrMaxShardsPerNodes $env:SOLR_MAX_SHARDS_NUMBER_PER_NODES `
    -SolrXdbSchemaFile .\\data\\schema.json `
    -SolrCollectionsToDeploy $env:SOLR_COLLECTIONS_TO_DEPLOY

Enable-Suggester -index "sitecore_sxa_master_index"
Enable-Suggester -index "sitecore_sxa_web_index"