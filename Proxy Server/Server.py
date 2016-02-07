#region -----------------Info-----------------
#Name:
#Version:
#By: Yaniv Sharon
#endregion -----------------Info-----------------

#region -----------------Imports-----------------
from Settings import *
from Printer import *
from Singleton import *
from Queue import *
from threading import *
import time
import socket
#endregion -----------------Imports-----------------

#region -----------------Constants-----------------

#endregion -----------------Constants-----------------

#region -----------------Class-----------------


class Server(object):
    __metaclass__ = Singleton
    settings = Settings()
    printer = Printer()
    __server_socket = socket.socket()
    __input_queue = Queue()
    __output_queue = Queue()
    __connect_lock = Lock()
    __close = False

    def __init__(self):
        self.__connectToServer()
        self.runThreads()

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
        while self.__input_queue.empty():
            time.sleep(0.1)
        return self.__input_queue.get()

    def closeThreads(self):
        self.__close = True
        self.send('closing')
        self.__server_socket.close()
        self.__addInput('closing')

    def __serverListenerThread(self):
        buffer_size = int(self.settings.getSetting('buffer_size'))
        while not self.__close:
            try:
                self.__addInput(self.__server_socket.recv(buffer_size))
            except socket.error:
                self.__connectToServer()

    def __outputSenderThread(self):
        while not self.__close:
            try:
                self.__send(self.__output_queue.get())
            except socket.error:
                self.__connectToServer()

    def __addInput(self, input):
        self.__input_queue.put(input)

    def __send(self, message):
        self.__server_socket.send(message)

    def __connectToServer(self):
        acquired = self.__connect_lock.acquire()
        if acquired:
            try:
                self.__send('blah')
            except socket.error:
                self.printer.printMessage(self.__class__.__name__, 'Connecting to server')
                self.__server_socket = socket.socket()
                ip = self.settings.getSetting('main_server_ip')
                port = int(self.settings.getSetting('main_server_port'))
                connected = False
                while not connected:
                    connected = True
                    try:
                        self.__server_socket.connect((ip, port))
                    except socket.error:
                        connected = False
                self.printer.printMessage(self.__class__.__name__, 'Connected to server')
            self.__connect_lock.release()


#endregion -----------------Class-----------------