from dotenv import load_dotenv
import bcrypt
import os
import base64

def main():
    load_dotenv()
    UMAIL = input("Email: ")
    UPASS = input("Password: ")#
    UNAME = input("Name: ")
    UTYPE = input("Type (1 Admin, 2 Employee, 3 Cleaner): ")
    if UTYPE != "1" and UTYPE != "2" and UTYPE != "3":
        print("Fuck you I don't have the patience for this")
        quit()
    
    # Define a fixed salt. (random salt for now)
    base64salt = os.getenv("BCRYPT_SALT")
    fixed_salt = base64.b64decode(base64salt)
    
    # Hash the email and password using the fixed salt
    UMAIL_HASHED = bcrypt.hashpw(UMAIL.encode('utf-8'), fixed_salt)
    UPASS_HASHED = bcrypt.hashpw(UPASS.encode('utf-8'), fixed_salt)
    
    QUERY1 = str("INSERT INTO USER_CREDENTIALS (UMAIL_HASH, UPASS_HASH) VALUES (decode('" +  str(UMAIL_HASHED.hex()) + "', 'hex'), decode('" +  str(UPASS_HASHED.hex()) + "', 'hex'));")
    QUERY3 = str("INSERT INTO USERS (U_NAME, U_MAIL, U_TYPE) VALUES ('" + UNAME + "', '" + UMAIL + "', " + UTYPE + ");")
    
    QUERY = str("BEGIN;\n\n" + QUERY1 + "\n" + QUERY3 + "\n\nCOMMIT;")
    
    print(QUERY)
    return

if __name__ == "__main__":
    main()
