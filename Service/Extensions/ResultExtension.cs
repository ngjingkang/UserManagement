using FluentResults;
using Service.Errors;

namespace Service.Extensions
{
    public static class ResultExtension
    {
        public static Result OkOrUpdateFailed(this bool source)
           => source
               ? Result.Ok()
               : Result.Fail("Update Failed Error");

        public static Result<IEnumerable<TSource>?> OkOrNotFound<TSource>(this IEnumerable<TSource>? source)
               => source.HasData()
                   ? Result.Ok(source)
                   : Result.Fail(new NotFoundError());
    }
}
