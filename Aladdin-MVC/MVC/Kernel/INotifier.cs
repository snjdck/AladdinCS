using System;

namespace Aladdin.MVC
{
	interface INotifier
	{
		/// <returns>如果成功调度了事件，则值为 true。 值 false 表示失败或对事件调用了 preventDefault()</returns>
		bool notify(Enum msgName, object msgData=null);
	}
}
