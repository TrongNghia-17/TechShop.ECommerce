global using Asp.Versioning;
global using Asp.Versioning.ApiExplorer;
global using MediatR;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.OutputCaching;
global using Microsoft.AspNetCore.RateLimiting;
global using Microsoft.Extensions.Options;
global using Microsoft.Net.Http.Headers;
global using Microsoft.OpenApi.Models;
global using Scalar.AspNetCore;
global using Serilog;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System.Runtime.ExceptionServices;
global using System.Threading.RateLimiting;
global using TechShop.ECommerce.Api.Extensions;
global using TechShop.ECommerce.Api.Middleware;
global using TechShop.ECommerce.Api.Models;
global using TechShop.ECommerce.Api.Swagger;
global using TechShop.ECommerce.Application;
global using TechShop.ECommerce.Application.Contracts.Identity;
global using TechShop.ECommerce.Application.Exceptions;
global using TechShop.ECommerce.Application.Features.Products.Commands.Create;
global using TechShop.ECommerce.Application.Features.Products.Commands.Delete;
global using TechShop.ECommerce.Application.Features.Products.Commands.Update;
global using TechShop.ECommerce.Application.Features.Products.Dtos;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetAll;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetProductsPaged;
global using TechShop.ECommerce.Application.Models;
global using TechShop.ECommerce.Application.Models.Identity;
global using TechShop.ECommerce.Identity;
global using TechShop.ECommerce.Infrastructure;
global using TechShop.ECommerce.Persistence;



