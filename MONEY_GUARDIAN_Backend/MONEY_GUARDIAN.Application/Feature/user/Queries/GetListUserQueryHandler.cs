using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Enums;
using MONEY_GUARDIAN.Domain.Helpers;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.user.Queries
{
    public class GetListUserQueryHandler(
        IQueryWrapper queryWrapper
    ) : IRequestHandler<GetListUserQuery, List<UserDto>>
    {
        public async Task<List<UserDto>> Handle(
            GetListUserQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<User> listUser =
                await queryWrapper
                    .QueryAsync<User>(
                        ItemsMessageConstants.GetUsers
                            .GetDescription(),
                        new
                        { },
                        FieldFilterHelper.BuildQueryArgs(listFilters)
                    );


            return MapListUsersToListUsersDto(listUser.ToList());
        }

        private static List<UserDto> MapListUsersToListUsersDto(List<User> listUser)
        {
            return listUser.Select(user =>
                new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.Username
                }
            ).ToList();
        }
    }
}
