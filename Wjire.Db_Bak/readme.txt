

配置文件优先级:

appsettings.Development.json > appsettings.json

{
  "connectionStrings": {
    "NCovRead": {
      "connectionString": "Data Source=localhost;Initial Catalog=nCov;Persist Security Info=True;User ID=sa;Password=1",
      "providerName": "sql"
    },
    "NCovWrite": {
      "connectionString": "Data Source=localhost;Initial Catalog=nCov;Persist Security Info=True;User ID=sa;Password=1",
      "providerName": "sql"
    }
  }
}