using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts.ReadWrite.Objects;

public class Login
{
    public static bool CheckPassword(string username, string password)
    {
        // Get infos from user file
        List<Users> users = Engine.ReadList<Users>(SerializationTypes.Users);

        foreach (Users user in users)
        {
            // check if the couple password / username is right
            if (user.userName == username && user.password == password)
            {
                Engine.UserId = user.userId;
                Engine.UserName = user.userName;
                Engine.MailAddress = user.email;
                return true;
            }
        }

        return false;
    }

    public static bool CreateNewUser(string username, string password, string email)
    {
        // Load infos from user file
        List<Users> users = Engine.ReadList<Users>(SerializationTypes.Users);
        List<string> ids = new List<string>(users.Select(x => x.userId));

        // Create new id
        string userId = Users.CreateNewId();

        // Ensure it is unique
        int attempt = 0;
        while (!IsUnique(userId, ids))
        {
            if (attempt >= 10)
            {
                Debug.Log("Could not create a new Unique User ID");
                return false;
            }

            userId = Users.CreateNewId();
            attempt += 1;
        }


        // add user to user list
        users.Add(new Users(userId, email, username, password));

        // save json
        Engine.Save<Users>(users, SerializationTypes.Users);

        Engine.UserId = userId;
        return true;
    }

    // TO BE COMPLETED -> SHOULD BE AN API VERIFYING THE DATABASE ONLINE
    private static bool IsUnique(string userId, List<string> ids)
    {
        foreach (string id in ids)
        {
            if (id == userId)
                return false;
        }

        return true;
    }
}
