variable "resource_group_name" {
  description = "Nome do Resource Group no Azure"
  type        = string
  default     = "rg-burguer404-Catalogo"
}

variable "location" {
  description = "Localização dos recursos no Azure"
  type        = string
  default     = "West US 2"
}

variable "aks_cluster_name" {
  description = "Nome do cluster AKS"
  type        = string
  default     = "aks-burguer404-Catalogo"
}

variable "dns_prefix" {
  description = "Prefixo DNS para o cluster AKS"
  type        = string
  default     = "aksburguer404Catalogo"
}

variable "node_count" {
  description = "Número de nós no node pool padrão"
  type        = number
  default     = 1
}

variable "vm_size" {
  description = "Tamanho da VM para os nós do AKS"
  type        = string
  default     = "Standard_DC2s_v3"
}

variable "environment" {
  description = "Ambiente (dev, staging, prod)"
  type        = string
  default     = "dev"
}

variable "create_resource_group" {
  description = "Se deve criar o resource group ou usar existente"
  type        = bool
  default     = true
}
