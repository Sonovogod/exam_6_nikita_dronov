
using FluentValidation.Results;

namespace HttpServer.dto;

public class ResponseDto<T>
{
    public T Result { get; set; }
    public List<ValidationFailure> Errors { get; set; } = new();
}