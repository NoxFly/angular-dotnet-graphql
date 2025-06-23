namespace Core.Helpers;

public class HttpException(int statusCode, string? message = "", object? details = null, Exception? innerException = null): Exception(message, innerException)
{
    public int StatusCode { get; set; } = statusCode;
    public object? Details { get; set; } = details;
}

public class BadRequestException(string? message = "", object? details = null, Exception? innerException = null): HttpException(400, message, details, innerException) {}
public class UnauthorizedException(string? message = "", object? details = null, Exception? innerException = null): HttpException(401, message, details, innerException) {}
public class ForbiddenException(string? message = "", object? details = null, Exception? innerException = null): HttpException(403, message, details, innerException) {}
public class NotFoundException(string? message = "", object? details = null, Exception? innerException = null): HttpException(404, message, details, innerException) {}
public class MethodNotAllowedException(string? message = "", object? details = null, Exception? innerException = null): HttpException(405, message, details, innerException) {}
public class NotAcceptableException(string? message = "", object? details = null, Exception? innerException = null): HttpException(406, message, details, innerException) {}
public class ConflictException(string? message = "", object? details = null, Exception? innerException = null): HttpException(409, message, details, innerException) {}
public class InternalServerErrorException(string? message = "", object? details = null, Exception? innerException = null): HttpException(500, message, details, innerException) {}
public class NotImplementedException(string? message = "", object? details = null, Exception? innerException = null): HttpException(501, message, details, innerException) {}
public class ServiceUnavailableException(string? message = "", object? details = null, Exception? innerException = null): HttpException(503, message, details, innerException) {}