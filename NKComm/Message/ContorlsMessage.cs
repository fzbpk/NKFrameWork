using NK.ENum;
namespace NK.Message
{
    /// <summary>
    /// 控件显示语言
    /// </summary>
    public partial class ContorlsMessage
    {
        /// <summary>
        /// 请选择
        /// </summary>
        /// <param name="message"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Select(string message="", Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "请选择"+ message;
                default:
                    return "Please select "+ message;
            } 
        }

        /// <summary>
        /// 请输入
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Enter(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "请输入";
                default:
                    return "Please enter";
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Search(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "查询";
                default:
                    return "Search";
            }
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Open(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "打开";
                default:
                    return "Open";
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Close(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "关闭";
                default:
                    return "Close";
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Exit(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "退出";
                default:
                    return "Exit";
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Save(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "保存";
                default:
                    return "Save";
            }
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string SaveAs(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "另存为";
                default:
                    return "Save As";
            }
        }

        /// <summary>
        /// 打开为
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string OpenAs(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "打开为";
                default:
                    return "Open As";
            }
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ADD(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "增加";
                default:
                    return "ADD";
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Delete(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "删除";
                default:
                    return "Delete";
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Edit(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "修改";
                default:
                    return "Edit";
            }
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string View(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "查看";
                default:
                    return "view";
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Creat(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "创建";
                default:
                    return "Creat";
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Import(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "导入";
                default:
                    return "Import";
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Export(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "导出";
                default:
                    return "Export";
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Print(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "打印";
                default:
                    return "Print";
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Set(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "设置";
                default:
                    return "Set";
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Clear(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "清除";
                default:
                    return "Clear";
            }
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string OK(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "确认";
                default:
                    return "OK";
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Cancel(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "取消";
                default:
                    return "Cancel";
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Back(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "返回";
                default:
                    return "Back";
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ReSet(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "重置";
                default:
                    return "ReSet";
            }
        }
        
        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Preview(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "预览";
                default:
                    return "Preview";
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Name(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "名称";
                default:
                    return "Name";
            }
        }

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Code(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "编码";
                default:
                    return "Code";
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string StartTime(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "开始时间";
                default:
                    return "StartTime";
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string EndTime(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "结束时间";
                default:
                    return "EndTime";
            }
        }

        /// <summary>
        /// 密码永不过期
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string PasswordNeverExpired(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "密码永不过期";
                default:
                    return "Password  Never Expired";
            }
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ForgoutPassword(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "忘记密码";
                default:
                    return "Forgout Password";
            }
        }
         
        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Enable(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "启用";
                default:
                    return "Enable";
            }
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Disable(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "禁用";
                default:
                    return "Disable";
            }
        }

        /// <summary>
        /// 关键字
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string KeyWord(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "关键字";
                default:
                    return "KeyWord";
            }
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string First(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "首页";
                default:
                    return "First";
            }
        }

        /// <summary>
        /// 末页
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Last(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "末页";
                default:
                    return "Last";
            }
        }

        /// <summary>
        /// 下页
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Next(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "下页";
                default:
                    return "Next";
            }
        }

        /// <summary>
        /// 上页
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Previous(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "上页";
                default:
                    return "Previous";
            }
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string Operation(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "操作";
                default:
                    return "Operation";
            }
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string List(Language language = Language.Chinese)
        {
            switch (language)
            {
                case Language.Chinese:
                    return "列表";
                default:
                    return "List";
            }
        }

    }
}
