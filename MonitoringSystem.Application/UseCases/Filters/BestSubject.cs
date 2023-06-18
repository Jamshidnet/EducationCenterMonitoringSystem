using AutoMapper;
using MediatR;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Application.UseCases.Subjects.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.Filters
{

    public record BestSubjectQuery(Guid TeacherId, decimal MinMark, decimal TakeNum, int PageNumber = 1, int PageSize = 10)
        : IRequest<PaginatedList<SubjectDto>>;

    public class BestSubjectQueryHandler : IRequestHandler<BestSubjectQuery, PaginatedList<SubjectDto>>
    {


        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public BestSubjectQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedList<SubjectDto>> Handle(BestSubjectQuery request, CancellationToken cancellationToken)
        {

            var FilteredSubjects = from subject in _dbContext.Subjects
                                   join mark in _dbContext.Grades
                                   on subject.Id equals mark.SubjectId
                                   where mark.GradeNum > request.MinMark
                                   && subject.TeacherId == request.TeacherId
                                   select subject;



            FilteredSubjects = FilteredSubjects.OrderBy(x => x.SubjectName);

            var SubjectList = FilteredSubjects.ToList();
            List<Subject> MatchedSubjects = new();

            int Count = 0;
            for (int i = 0; i < SubjectList.Count - 2; i++)
            {
                Count++;

                if (SubjectList[i] != SubjectList[i+1])
                {
                    if(Count>=request.TakeNum)
                        MatchedSubjects.Add(SubjectList[i]);
                    Count = 0;
                }
            }

            IEnumerable<SubjectDto> result = _mapper.Map<SubjectDto[]>(MatchedSubjects);

            PaginatedList<SubjectDto> paginatedList =
                 PaginatedList<SubjectDto>.CreateAsync(
                    result, request.PageNumber, request.PageSize);

            return paginatedList;
        }


    }

}
