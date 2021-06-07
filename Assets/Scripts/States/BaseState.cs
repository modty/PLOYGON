using UnityEngine;

namespace States
{
    /// <summary>
    /// 行为基类，只对行为进行某些记录，其中方法只能通过子对象调用。
    /// 子类继承后，需要重写DoAction方法添加行为
    /// </summary>
    public abstract class BaseState
    {
        private float time_start;
        private float time_end;
        private float time_duration;
        private bool enterExit;
        protected bool stop;
        protected BaseState()
        {
            time_start = 0;
            time_end = 0;
            enterExit = true;
            stop = true;
        }
        protected void BeforeUpdate()
        {
            //Debug.Log("MovementAction进入刷新");
            RecordTime();
        }
        protected void AfterUpdate()
        {
            RecordTime();
            //Debug.Log("MovementAction退出刷新："+time_duration);
        }
        /// <summary>
        /// 时间记录，进入和退出都会调用进行记录，通过bool变量进行控制。
        /// </summary>
        private void RecordTime()
        {
            if (enterExit)
            {
                time_start = Time.time;
            }
            else
            {
                time_end = Time.time;
                time_duration = time_end - time_start;
            }

            enterExit = !enterExit;
        }

        /// <summary>
        /// 供外部每帧调用，BeforeUpdate、DoUpdate、AfterUpdate三个方法依次执行。
        /// 一般情况下，不建议重写，避免逻辑混乱，默认状态为true
        /// </summary>
        public void Update()
        {
            if (!stop)
            {
                BeforeUpdate();

                DoUpdate();
                
                AfterUpdate();

            }
        }
        /// <summary>
        /// 只能通过子对象进行控制，不提供外部控制
        /// </summary>
        protected void StopAction()
        {
            stop = true;
        }

        protected void StartAction()
        {
            stop = false;
            Update();
        }
        protected abstract void DoUpdate();
    }
}