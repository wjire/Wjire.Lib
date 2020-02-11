using Wjire.Db;

namespace ConsoleTest
{
    public partial class NCovDbFactory
    {

        private static readonly string NCovRead = "NCovRead";
        private static readonly string NCovWrite = "NCovWrite";


        /// <summary>
        /// 创建 IUserInfoRepository 读连接
        /// </summary>
        /// <returns></returns>
        public static UserInfoRepository CreateIUserInfoRepositoryRead()
        {
            return new UserInfoRepository(NCovRead);
        }

        /// <summary>
        /// 创建 IUserInfoRepository 读连接
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static UserInfoRepository CreateIUserInfoRepositoryRead(IUnitOfWork unit)
        {
            return new UserInfoRepository(unit);
        }

        /// <summary>
        /// 创建 IUserInfoRepository 写连接
        /// </summary>
        /// <returns></returns>
        public static UserInfoRepository CreateIUserInfoRepositoryWrite()
        {
            return new UserInfoRepository(NCovWrite);
        }

        /// <summary>
        /// 创建 IUserInfoRepository 写连接
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static UserInfoRepository CreateIUserInfoRepositoryWrite(IUnitOfWork unit)
        {
            return new UserInfoRepository(unit);
        }

    }
}