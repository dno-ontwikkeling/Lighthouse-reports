param(
    [Parameter(mandatory = $true)][string]$url, 
    [Parameter(mandatory = $true)][string]$folder)


lighthouse $url --only-categories="performance,accessibility,best-practices,seo" --output="json,html" --output-path=$($Folder)phone --quiet --chrome-flags="--headless"
lighthouse $url --only-categories="performance,accessibility,best-practices,seo" --output="json,html" --output-path=$($Folder)desktop --quiet --chrome-flags="--headless" --preset desktop
       

