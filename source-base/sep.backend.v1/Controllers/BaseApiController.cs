using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using sep.backend.v1.Common.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController<T> : ControllerBase
{
    protected readonly ILogger<T> _logger;

    protected BaseApiController(ILogger<T> logger)
    {
        _logger = logger;
    }

    protected string UserEmail => HttpContext.Items["UserEmail"]?.ToString();
    protected string UserRole => HttpContext.Items["UserRole"]?.ToString();
    protected int? SchoolId
    {
        get
        {
            if (HttpContext.Items["SchoolId"] != null && int.TryParse(HttpContext.Items["SchoolId"].ToString(), out int schoolId))
            {
                return schoolId;
            }
            return null;
        }
    }

    protected int? SchoolAdminId
    {
        get
        {
            if (HttpContext.Items["SchoolAdminId"] != null && int.TryParse(HttpContext.Items["SchoolAdminId"].ToString(), out int schoolAdminId))
            {
                return schoolAdminId;
            }
            return null;
        }
    }

    protected int? PupilId
    {
        get
        {
            if (HttpContext.Items["PupilId"] != null && int.TryParse(HttpContext.Items["PupilId"].ToString(), out int pupilId))
            {
                return pupilId;
            }
            return null;
        }
    }
    protected int? TeacherId
    {
        get
        {
            if (HttpContext.Items["TeacherId"] != null && int.TryParse(HttpContext.Items["TeacherId"].ToString(), out int teacherId))
            {
                return teacherId;
            }
            return null;
        }
    }

    protected int? BusSupervisorId
    {
        get
        {
            if (HttpContext.Items["BusSupervisorId"] != null && int.TryParse(HttpContext.Items["BusSupervisorId"].ToString(), out int busSupervisorId))
            {
                return busSupervisorId;
            }
            return null;
        }
    }


    protected IActionResult HandleException(Exception e, string message = "An unexpected error occurred.", Dictionary<string, string[]>? errors = null)
    {
        _logger.LogError(e, message);

        var errorMessages = errors ?? new Dictionary<string, string[]> { { "General", new[] { e.Message } } };

        var response = ApiResponse<string>.Error(message, errorMessages);

        return StatusCode(StatusCodes.Status500InternalServerError, response);
    }

    protected IActionResult HandleSuccess<TData>(TData? data = default, string? message = null, int statusCode = StatusCodes.Status200OK)
    {
        var responseData = data ?? default!;
        if (responseData is IEnumerable<object> collection && !collection.Any())
        {
            responseData = (TData)(object)Array.Empty<object>();
        }

        var responseMessage = message ?? "Success";
        var response = ApiResponse<TData>.Success(responseData, responseMessage);

        return StatusCode(statusCode, response);
    }

    protected IActionResult HandleSuccessWithNoContent(string? message = null)
    {
        var responseMessage = message ?? "No content.";
        return StatusCode(StatusCodes.Status204NoContent, new { message = responseMessage });
    }

    protected IActionResult HandleBadRequest(string message, string key = "General")
    {
        var errors = new Dictionary<string, string[]>();

        errors[key] = new[] { message };

        var errorResponse = new
        {
            data = (object)null,
            succeeded = false,
            errors = errors,
            message = "Bad Request"
        };

        return BadRequest(errorResponse);
    }

    protected IActionResult HandleConflict(string message)
    {
        var errorResponse = new
        {
            data = (object)null,
            succeeded = false,
            errors = new
            {
                General = new[] { message }
            },
            message = "Conflict"
        };

        return Conflict(errorResponse);
    }

    protected IActionResult HandleNotFound(string message)
    {
        var errorResponse = new
        {
            data = (object)null,
            succeeded = false,
            errors = new
            {
                errors = new[] { message }
            },
            message = "Not Found"
        };

        return NotFound(errorResponse);
    }

    protected IActionResult HandleModelStateErrors(ModelStateDictionary modelState)
    {
        var camelCaseErrors = modelState.ToDictionary(
            kvp => JsonNamingPolicy.CamelCase.ConvertName(kvp.Key),
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );

        var response = ApiResponse<object>.Error("Validation failed.", camelCaseErrors);
        return BadRequest(response);
    }

    protected IActionResult HandleError(Exception ex, string message = "An unexpected error occurred.")
    {
        var errorResponse = new
        {
            data = (object)null,
            succeeded = false,
            errors = new
            {
                General = new[] { ex.Message }
            },
            message
        };

        return StatusCode(500, errorResponse);
    }

    protected IActionResult HandleUnauthorized(string message)
    {
        var errorResponse = new
        {
            data = (object)null,
            succeeded = false,
            errors = new
            {
                General = new[] { message }
            },
            message = "Unauthorized"
        };

        return Unauthorized(errorResponse);
    }
}