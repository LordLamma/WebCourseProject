﻿namespace GameCatalogue.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class EntityValidationConstants
    {
        public static class Genre
        {
            public const int NameMinLenght = 2;
            public const int NameMaxLenght = 25;
        }

        public static class Game
        {
            public const int NameMinLenght = 1;
            public const int NameMaxLenght = 180;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLenght = 300;

            public const int ImageURLMaxLength = 2048;
        }

        public static class Developer
        {
            public const string EmailRegEx = "(.*)[@](.*)\\.(.*)";
            
            public const int EmailMaxLenght = 320;
            public const int EmailMinLenght = 6;
        }

        public static class User
        {
            public const int DisplayNameMinLength = 3;
            public const int DisplayNameMaxLength = 30;

            public const int PassWordMinLength = 6;
			public const int PassWordMaxLength = 100;
		}

        public static class Comment
        {
            public const int MessageMinLenght = 4;
            public const int MessageMaxLength = 200;
        }
    }
}
