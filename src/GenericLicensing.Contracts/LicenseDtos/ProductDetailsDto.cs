﻿using System.ComponentModel.DataAnnotations;

namespace GenericLicensing.Contracts.LicenseDtos;

public class ProductDetailsDto
{
  [Required] public string ProductId { get; set; }
  [Required] public string ProductName { get; set; }
  [Required] public ProductAttributesDto ProductAttributes { get; set; }
}