using AxGrid.Model;

namespace AxGrid {
    public static class Settings {
        /// <summary>
        /// Модель
        /// </summary>
        public static DynamicModel Model { get; set; } = new SimpleModel();
        
        /// <summary>
        /// Языки
        /// </summary>
        public static string[] Languages { get; set; } = {"ru"};
        
        /// <summary>
        /// Машина состояний
        /// </summary>
        public static FSM.FSM Fsm { get; set; }
    }
}