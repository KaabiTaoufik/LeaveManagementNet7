output "connection_string" {
    value = azurerm_linux_web_app.web_app.connection_string
    sensitive = true
}