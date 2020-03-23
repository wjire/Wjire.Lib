using System;
using System.ComponentModel.DataAnnotations;

namespace Wjire.Excel.Test.Console
{
	/// <summary>
	/// MaintainerSalarySource
	/// </summary>
	public class MaintainerSalarySource
	{

		/// <summary>
		/// 分公司名称
		/// </summary>
		public string BranchCompanyName { get; set; }


		/// <summary>
		/// 代维公司名称
		/// </summary>
		public string AgentCompanyName { get; set; }


		/// <summary>
		/// 账号
		/// </summary>
		public string Account { get; set; }


		/// <summary>
		/// 姓名
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// 岗位
		/// </summary>
		public string RoleName { get; set; }


		/// <summary>
		/// 班组
		/// </summary>
		public string Team { get; set; }


		/// <summary>
		/// 户线到户
		/// </summary>
		public int RLineToHouse { get; set; }


		/// <summary>
		/// 多层-户线未到户(11楼及以下)
		/// </summary>
		public int RMultiLevel { get; set; }


		/// <summary>
		/// 高层-户线未到户(11楼以上)
		/// </summary>
		public int RHighLevel { get; set; }


		/// <summary>
		/// 自建城中村、别墅
		/// </summary>
		public int RSelfBuilt { get; set; }


		/// <summary>
		/// 医院酒店
		/// </summary>
		public int RHotelAndHospital { get; set; }


		/// <summary>
		/// 临街商铺
		/// </summary>
		public int SRtreetShops { get; set; }


		/// <summary>
		/// 学校
		/// </summary>
		public int RSchool { get; set; }


		/// <summary>
		/// 农村散居
		/// </summary>
		public int RRuralArea { get; set; }


		/// <summary>
		/// 集中居住
		/// </summary>
		public int RConcentratedResidence { get; set; }


		/// <summary>
		/// 综合市场及商业综合体
		/// </summary>
		public int RCommercialComplex { get; set; }


		/// <summary>
		/// 出勤天数
		/// </summary>
		public int Days { get; set; }


		/// <summary>
		/// 底薪金额总计
		/// </summary>
		public decimal RTotal { get; set; }


		/// <summary>
		/// 户线到户
		/// </summary>
		public int FLineToHouse { get; set; }


		/// <summary>
		/// 多层-户线未到户(11楼及以下)
		/// </summary>
		public int FMultiLevel { get; set; }


		/// <summary>
		/// 高层-户线未到户(11楼以上)
		/// </summary>
		public int FHighLevel { get; set; }


		/// <summary>
		/// 自建城中村、别墅
		/// </summary>
		public int FSelfBuilt { get; set; }


		/// <summary>
		/// 医院酒店
		/// </summary>
		public int FHotelAndHospital { get; set; }


		/// <summary>
		/// 临街商铺
		/// </summary>
		public int FStreetShops { get; set; }


		/// <summary>
		/// 学校
		/// </summary>
		public int FSchool { get; set; }


		/// <summary>
		/// 农村散居
		/// </summary>
		public int FRuralArea { get; set; }


		/// <summary>
		/// 集中居住
		/// </summary>
		public int FConcentratedResidence { get; set; }


		/// <summary>
		/// 综合市场及商业综合体
		/// </summary>
		public int FCommercialComplex { get; set; }


		/// <summary>
		/// 装移机金额总计
		/// </summary>
		public decimal FTotal { get; set; }


		/// <summary>
		/// 加装ITV和IMS，安装多台电视
		/// </summary>
		public int InstallItvAndIms { get; set; }


		/// <summary>
		/// 智能组网
		/// </summary>
		public int IntelligentNetworking { get; set; }


		/// <summary>
		/// 二级分光器扩容安装
		/// </summary>
		public int InstallTwoStage { get; set; }


		/// <summary>
		/// 198套餐以上安装补贴
		/// </summary>
		public int Install198Package { get; set; }


		/// <summary>
		/// 按次-安全隐患处理
		/// </summary>
		public int SafetyTreatment { get; set; }


		/// <summary>
		/// 按次-配合网络调整
		/// </summary>
		public int NetworkAdjustment { get; set; }


		/// <summary>
		/// 按次-家客巡检
		/// </summary>
		public int Patrolling { get; set; }


		/// <summary>
		/// 按次-应急通信保障
		/// </summary>
		public int EmergencyCommunication { get; set; }


		/// <summary>
		/// 附加服务金额总计
		/// </summary>
		public decimal ATotal { get; set; }


		/// <summary>
		/// 话费补贴
		/// </summary>
		public decimal TelephoneSubsidy { get; set; }


		/// <summary>
		/// 分公司奖励金额
		/// </summary>
		public decimal BranchCompanyAward { get; set; }


		/// <summary>
		/// 分公司考核金额
		/// </summary>
		public decimal BranchCompanyExamine { get; set; }


		/// <summary>
		/// 全业支激励金额
		/// </summary>
		public decimal BranchIncentive { get; set; }


		/// <summary>
		/// 应发薪酬合计
		/// </summary>
		public decimal GrossPay { get; set; }


		/// <summary>
		/// 随销酬金
		/// </summary>
		public decimal Follow { get; set; }


		/// <summary>
		/// 税金
		/// </summary>
		public decimal Tax { get; set; }


		/// <summary>
		/// 社保
		/// </summary>
		public decimal SocialSecurity { get; set; }


		/// <summary>
		/// 实发金额
		/// </summary>
		public decimal Actual { get; set; }


		/// <summary>
		/// 公司社保
		/// </summary>
		public decimal CompanySocialSecurity { get; set; }


		/// <summary>
		/// 补结费用
		/// </summary>
		public decimal Supplementary { get; set; }


		/// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; }
    }
}
