using FluentValidation;

namespace KeyCrawler.WebApi.V1.Requests
{
    public class SearchRequestValidator : AbstractValidator<SearchRequest>
    {
        public SearchRequestValidator() 
        {
            RuleForEach(r => r.Domains).NotEmpty()
                                       .Matches(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)")
                                       .WithMessage("Damain must bust a an URL");
            RuleForEach(r => r.Keywords).NotEmpty();
        }
    }
}
