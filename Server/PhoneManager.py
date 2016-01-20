#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Singleton import *
from Settings import *
from Printer import *
from Server import *
from Phone import *
from threading import Thread
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class PhoneManager(object):
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    server = Server()
    __phone_dict = {}
    __close = False

    def addPhone(self, phone_socket):
        phone = Phone(phone_socket)
        add_phone_thread = Thread(target=self.__addPhoneToDict, args=phone)
        add_phone_thread.start()

    def getPhone(self, username):
        return self.__phone_dict[username]

    def runThread(self):
        while not self.__close:
            server_message = self.server.recv()
            if server_message is not None:
                if ':' in server_message:
                    phone_username, message = server_message.split(':', 1)
                    phone = self.getPhone(phone_username)
                    phone.send(message)

    def __addPhoneToDict(self, phone):
        phone.establishConnection()
        username = phone.getUsername()
        self.__phone_dict[username] = phone
        phone_thread = Thread(target=phone.runThread)
        phone_thread.start()

    def closeThread(self):
        self.__close = True


#endregion -----------------Class-----------------