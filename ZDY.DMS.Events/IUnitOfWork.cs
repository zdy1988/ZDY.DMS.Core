using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdy.Events
{
    /// <summary>
    /// 单元工作，用于事务原子性
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 获得一个<see cref="System.Boolean"/>值，该值表述了当前的Unit Of Work事务是否已被提交。
        /// </summary>
        bool Committed { get; }
        /// <summary>
        /// 提交当前的Unit Of Work事务。
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚当前的Unit Of Work事务。
        /// </summary>
        void Rollback();
    }
}
