using System.Collections.Generic;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// Implements <see cref="IPagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="Items"/> list</typeparam>
    public class PagedResultDto<T> : IPagedResult<T>, IDto
    {
        public IReadOnlyList<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }
        private IReadOnlyList<T> _items;

        public int TotalCount { get; set; }
    }
}