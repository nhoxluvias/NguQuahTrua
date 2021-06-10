﻿using Common.Mail;
using Data.Common.Hash;
using System;

namespace Web.Common
{
    public class ConfirmCode
    {
        public string Send(string emailAddress)
        {
            string confirmCode = new Random().NextStringOnlyNumericCharacter(8);
            string message = string.Format("Mã xác nhận của bạn là: {0}", confirmCode);
            new EMail().Send(emailAddress, "Mã xác nhận tài khoản", message);
            return confirmCode;
        }

        public string CreateToken(int length = 30)
        {
            string randomString = new Random().NextString(20);
            PBKDF2_Hash pbkdf2 = new PBKDF2_Hash();
            return pbkdf2.Hash(randomString, "confirmToken", length);
        }
    }
}