using Godot;

namespace MkGames {
    public class Timer : Node {

        public event CallbackDelegate OnCounterFinished;
        public event CallbackDelegate OnSecondElapsed;
        public bool CounterFinished = false;
        public bool OneShot = false;
        public bool Started = false;
        public float TimeSinceStarted { get; private set; }
        private float m_counter;
        private float m_counterSet;

        private bool m_secondElapsed;
        public bool SecondElapsed {
            get {
                if (m_secondElapsed) {
                    m_secondElapsed = false;
                    return true;
                }
                return false;
            }
            private set { m_secondElapsed = value; }
        }

        public Timer (Node attachToNode, float time) {
            attachToNode.AddChild (this);
            m_counterSet = time;
        }

        public override void _Process (float delta) {
            if ((!CounterFinished || !OneShot) && Started) {
                Update (delta);
            }

        }

        public void Start () {
            Started = true;
        }

        public void Restart () {
            CounterFinished = false;
        }

        public void SetDownCounter (float time) {
            m_counterSet = time;
        }

        public void Update (float delta) {
            if (Mathf.Floor (TimeSinceStarted + delta) > Mathf.Floor (TimeSinceStarted)) {
                SecondElapsed = true;
                OnSecondElapsed?.Invoke ();
            }

            m_counter -= delta;
            TimeSinceStarted += delta;
            if (m_counter < 0) {
                CounterFinished = true;
                OnCounterFinished?.Invoke ();
                m_counter = m_counterSet;
            }

        }

    }
    
    public delegate void CallbackDelegate ();
}