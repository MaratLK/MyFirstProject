﻿namespace PLK_TwoTry_Back.Models
{
    public class NewsImage
    {
        public int NewsImageID { get; set; }
        public string ImageUrl { get; set; }

        public int NewsID { get; set; }  // Это обязательное поле, не nullable

        // Навигационное свойство для связи с новостью
        public News News { get; set; }
    }


}
