// MLTimerDot.h

#pragma once

using namespace System;

namespace MLTimerDot {

	public ref class Class1
	{
		// TODO: Add your methods for this class here.
	};

	//得到计算机启动到现在的时钟周期
	unsigned __int64 GetCycleCount(void)
	{
		_asm  _emit 0x0F
		_asm  _emit 0x31
	}
	//声明 .NET 类
	public ref class MLTimer
	{
	protected:
		UInt64 m_startcycle;
		UInt64 m_overhead;

	public:
		MLTimer(void)
		{
			//为了计算更精确取得调用一个 GetCycleCount() 的时钟周期
			m_overhead = 0;
			Start();
			m_overhead = Stop();
		}

		UInt64 Stop(void)
		{
			return GetCycleCount() - m_startcycle - m_overhead;
		}

		void Start(void)
		{
			m_startcycle = GetCycleCount();
		}

		virtual UInt64 get_Overhead()
		{
			return m_overhead;
		}
	};
}
