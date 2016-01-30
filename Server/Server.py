#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Settings import *
from Printer import *
from Singleton import *
from socket import *
from Queue import *
from threading import Thread
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Server(object):
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    __server_socket = socket()
    __input_queue = Queue()
    __output_queue = Queue()
    __close = False

    def __init__(self):
        ip = self.settings.getSetting('main_server_ip')
        port = int(self.settings.getSetting('main_server_port'))
        connected = False
        while not connected:
            connected = True
            try:
                self.__server_socket.connect((ip, port))
            except Exception:
                connected = False
        self.printer.printMessage(self.__class__.__name__, 'Connected to server')

    def runThreads(self):
        server_listener_thread = Thread(name='server_listener_thread', target=self.__serverListenerThread)
        server_listener_thread.setDaemon(True)
        server_listener_thread.start()
        output_sender_thread = Thread(name='output_sender_thread', target=self.__outputSenderThread)
        output_sender_thread.setDaemon(True)
        output_sender_thread.start()

    def send(self, message):
        self.__output_queue.put(message)

    def recv(self):
        if self.__input_queue.not_empty:
            return self.__input_queue.get()
        else:
            return None

    def closeThreads(self):
        self.__close = True
        self.send('closing')
        self.__server_socket.close()
        self.__addInput('closing')

    def __serverListenerThread(self):
        buffer_size = self.settings.getSetting('buffer_size')
        while not self.__close:
            self.__addInput(self.__server_socket.recv(buffer_size))

    def __outputSenderThread(self):
        while not self.__close:
            self.__send(self.__output_queue.get())

    def __addInput(self, input):
        self.__input_queue.put(input)

    def __send(self, message):
        self.__server_socket.send(message)


#endregion -----------------Class-----------------