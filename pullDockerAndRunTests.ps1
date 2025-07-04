docker pull swaggerapi/petstore
$containerId = docker run -d -p 8080:8080 swaggerapi/petstore
try {
    dotnet test --logger "trx;LogFileName=TestResults.trx"
}
finally {
    docker stop $containerId | Out-Null
    docker rm $containerId | Out-Null
}