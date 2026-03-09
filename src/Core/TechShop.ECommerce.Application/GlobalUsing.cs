global using AutoMapper;
global using FluentValidation;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics;
global using System.Linq.Expressions;
global using System.Reflection;
global using TechShop.ECommerce.Application.BackgroundJobs.Emails;
global using TechShop.ECommerce.Application.Behaviors;
global using TechShop.ECommerce.Application.Common;
global using TechShop.ECommerce.Application.Common.Caching;
global using TechShop.ECommerce.Application.Common.Errors;
global using TechShop.ECommerce.Application.Common.Paging;
global using TechShop.ECommerce.Application.Contracts.Authentication;
global using TechShop.ECommerce.Application.Contracts.Caching;
global using TechShop.ECommerce.Application.Contracts.Email;
global using TechShop.ECommerce.Application.Contracts.Identity;
global using TechShop.ECommerce.Application.Contracts.Logging;
global using TechShop.ECommerce.Application.Contracts.PaymentGateway;
global using TechShop.ECommerce.Application.Contracts.Persistence;
global using TechShop.ECommerce.Application.Exceptions;
global using TechShop.ECommerce.Application.Features.Carts.Dtos;
global using TechShop.ECommerce.Domain.Entities.Cart;
global using TechShop.ECommerce.Domain.Entities.Catalog;
global using TechShop.ECommerce.Domain.Entities.Orders;
global using TechShop.ECommerce.Domain.Entities.Payments;
global using TechShop.ECommerce.Domain.Errors;
global using TechShop.ECommerce.Domain.Exceptions;
global using TechShop.ECommerce.Domain.ValueObjects;












