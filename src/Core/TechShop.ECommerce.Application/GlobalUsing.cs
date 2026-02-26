global using AutoMapper;
global using FluentValidation;
global using MediatR;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics;
global using System.Linq.Expressions;
global using System.Reflection;
global using TechShop.ECommerce.Application.Behaviors;
global using TechShop.ECommerce.Application.Common;
global using TechShop.ECommerce.Application.Common.Cursors;
global using TechShop.ECommerce.Application.Common.Offset;
global using TechShop.ECommerce.Application.Contracts.Authentication;
global using TechShop.ECommerce.Application.Contracts.Email;
global using TechShop.ECommerce.Application.Contracts.Identity;
global using TechShop.ECommerce.Application.Contracts.Logging;
global using TechShop.ECommerce.Application.Contracts.Persistence;
global using TechShop.ECommerce.Application.Exceptions;
global using TechShop.ECommerce.Application.Features.Carts.Dtos;
global using TechShop.ECommerce.Application.Features.Orders.Commands.PlaceOrder;
global using TechShop.ECommerce.Application.Features.Orders.Dtos;
global using TechShop.ECommerce.Application.Features.Orders.Events.OrderPlaced;
global using TechShop.ECommerce.Application.Features.Products.Commands.Create;
global using TechShop.ECommerce.Application.Features.Products.Dtos;
global using TechShop.ECommerce.Application.Features.Products.Queries.GetDetails;
global using TechShop.ECommerce.Application.Models.Email;
global using TechShop.ECommerce.Application.Models.Identity;
global using TechShop.ECommerce.Domain.Entities.Cart;
global using TechShop.ECommerce.Domain.Entities.Catalog;
global using TechShop.ECommerce.Domain.Entities.Orders;
global using TechShop.ECommerce.Domain.Errors;
global using TechShop.ECommerce.Domain.Exceptions;
global using TechShop.ECommerce.Domain.ValueObjects;









