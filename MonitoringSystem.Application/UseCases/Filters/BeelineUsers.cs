using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitoringSystem.Application.Common.Interfaces;
using MonitoringSystem.Application.Common.Models;
using MonitoringSystem.Application.UseCases.Students.Models;
using MonitoringSystem.Domein.Entities;

namespace MonitoringSystem.Application.UseCases.StudentsListBy20Age
{
    public  record  BeelineUsersQuery(int PageNumber=1, int PageSize = 10) : IRequest<PaginatedList<PersonDto>>;

    public class BeelineUsersQueryHandler : IRequestHandler<BeelineUsersQuery, PaginatedList<PersonDto>>
    { 
        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public BeelineUsersQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;

            _mapper = mapper;
        }

        public async Task<PaginatedList<PersonDto>> Handle(BeelineUsersQuery request, CancellationToken cancellationToken)
        {
            Student[] students = await _dbContext.Students
                .Include(x => x.Grades).ToArrayAsync();
            Teacher[] teachers = await _dbContext.Teachers
                .Include(x => x.Subjects).ToArrayAsync();


            List<PersonDto> people =  _mapper.Map<PersonDto[]>(students).ToList();
            people.AddRange (_mapper.Map<PersonDto[]>(teachers));

            var SortedPeople = from person in people
                               where person.PhoneNumber.IsBeeline()
                               select person;

          //  List < StudentDto > dtos = _mapper.Map<StudentDto[]>(SortedPeople).ToList();

            PaginatedList<PersonDto> paginatedList =
                 PaginatedList<PersonDto>.CreateAsync(
                    SortedPeople, request.PageNumber, request.PageSize);

            return paginatedList;


        }
      
    }
   


}
