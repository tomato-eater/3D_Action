namespace Playable.Alpha
{
    /// <summary>
    /// Alphaのアクション
    /// </summary>
    public class ActionController : ActionControllerBase
    {
        bool _counterTrigger = false;
        bool _isCounter = false;

        public override void UniqueStart()
        {
            MyAnimator.SetTrigger("Unique");
            _counterTrigger = false;
            _endUnique = false;
            _isCounter = false;
        }

        public override bool UniqueUpdate(float time)
        {
            return _endUnique;  
        }

        public override void UniqueEnd()
        {

        }

        public override bool Hit()
        {
            if (_counterTrigger)
            {
                _counterTrigger = false;
                _isCounter = true;
                MyAnimator.Play("CounterAttack");
                return true;
            }
            else if( _isCounter)
                return true;

            return false;
        }

        /// <summary>
        /// カウンターの有効化・無効化　アニメーターで制御
        /// </summary>
        /// <param name="trigger">1ならカウンターを有効化する、その他は無効</param>
        public void CounterTrigger(int trigger) => _counterTrigger = trigger == 1;
    }
}
