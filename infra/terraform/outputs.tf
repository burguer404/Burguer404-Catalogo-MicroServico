output "resource_group_name" {
  description = "Nome do Resource Group (criado ou existente)"
  value       = local.rg_name
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

