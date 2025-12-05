output "resource_group_name" {
  description = "Nome do Resource Group criado"
  value       = azurerm_resource_group.rg.name
}

output "aks_cluster_name" {
  description = "Nome do cluster AKS criado"
  value       = azurerm_kubernetes_cluster.aks.name
}

output "aks_cluster_location" {
  description = "Localização do cluster AKS"
  value       = azurerm_kubernetes_cluster.aks.location
}

output "kube_config" {
  description = "Configuração do Kubernetes (sensível)"
  value       = azurerm_kubernetes_cluster.aks.kube_config_raw
  sensitive   = true
}

