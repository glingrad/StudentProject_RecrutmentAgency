using System;
using System.Linq;
using System.Net;

namespace RecruitmentAgency.ApplicationData
{
    public static class AuditService
    {
        // Простой метод логирования без получения IP-адреса
        public static void LogAction(int userId, string actionName, string details = "")
        {

            // Получаем тип действия из базы или создаем новый
            var actionType = AppConnect.modelOdb.AuditActions.FirstOrDefault(a => a.ActionName == actionName);
            if (actionType == null)
            {
                actionType = new AuditActions { ActionName = actionName };
                AppConnect.modelOdb.AuditActions.Add(actionType);
                AppConnect.modelOdb.SaveChanges();
            }

               
            var logEntry = new AuditLogs
            {
               UserId = userId,
               ActionId = actionType.ActionId,
               ActionDateTime = DateTime.Now,
               Details = details,
               IpAddress = "127.0.0.1"
            };

            AppConnect.modelOdb.AuditLogs.Add(logEntry);
            AppConnect.modelOdb.SaveChanges();
            
        }

        // Упрощенные методы для частых операций
        public static void LogLogin(int userId, string email)
        {
            LogAction(userId, "Вход в систему", $"Пользователь {email} вошел в систему");
        }

        public static void LogUserCreation(int adminId, string newUserEmail, string role)
        {
            LogAction(adminId, "Создание пользователя", $"Создан пользователь {newUserEmail} с ролью {role}");
        }

        public static void LogUserUnlock(int adminId, string userEmail)
        {
            LogAction(adminId, "Разблокировка пользователя", $"Пользователь {userEmail} разблокирован");
        }
    }
}