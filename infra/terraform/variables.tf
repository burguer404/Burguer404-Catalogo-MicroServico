variable "resource_group_name" {
  description = "Nome do Resource Group no Azure"
  type        = string
  default     = "rg-burguer404"
}

variable "location" {
  description = "Localização dos recursos no Azure"
  type        = string
  default     = "East US"
}

variable "aks_cluster_name" {
  description = "Nome do cluster AKS"
  type        = string
  default     = "aks-burguer404"
}

variable "dns_prefix" {
  description = "Prefixo DNS para o cluster AKS"
  type        = string
  default     = "aksburguer404"
}

variable "node_count" {
  description = "Número de nós no node pool padrão"
  type        = number
  default     = 1
}

variable "vm_size" {
  description = "Tamanho da VM para os nós do AKS"
  type        = string
  default     = "Standard_D4s_v3"
}

variable "environment" {
  description = "Ambiente (dev, staging, prod)"
  type        = string
  default     = "dev"
}

