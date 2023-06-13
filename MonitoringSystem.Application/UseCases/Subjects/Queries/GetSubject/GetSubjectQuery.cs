using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace StudentPaymentSystem.Application.UseCases.Students.Queries.GetStudent;

public  record GetSubjectQuery(Guid Id) : IRequest<GetStudentsWithGrades>;

public class GetStudentQueryHandler : IRequestHandler<GetSubjectQuery, GetStudentsWithGrades>
{
    IApplicationDbContext _dbContext;
    IMapper _mapper;

    public GetStudentQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async Task<GetStudentsWithGrades> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
    {
        GetStudentsWithGrades student = FilterIfStudentExsists(request.Id);

        return _mapper.Map<GetStudentsWithGrades>(student);
    }

    private GetStudentsWithGrades FilterIfStudentExsists(Guid id)
    {
        Student? student = _dbContext.Students
            .Include(x=>x.Courses)
            .Include(x=>x.Payments)
            .FirstOrDefault(x => x.Id == id);

        CourseDto[] mappedSt = _mapper.Map<CourseDto[]>(student.Courses);
        PaymentDto[] payments = _mapper.Map<PaymentDto[]>(student.Payments);
        GetStudentDtoWithPayments getAllStudentDto = new()
        {
            FirstName=student.FirstName,
            LastName=student.LastName,
            Address=student.Address,
            Id=student.Id,
            Email=student.Email,
            PhoneNumber = student.PhoneNumber,
            Courses=mappedSt,
            Payments=payments
        };
        

        if (student is null)
        {
            throw new NotFoundException(" There is on student with this Id. ");
        }

        return getAllStudentDto;
    }


}