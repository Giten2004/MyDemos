#!/usr/bin/env python  
#-*-encoding:utf-8-*-
  
import unittest  
import myclass  
  
class mytest(unittest.TestCase):  
      
    ##初始化工作  
    def setUp(self):  
        pass  
      
    #退出清理工作  
    def tearDown(self):  
        pass  
      
    #具体的测试用例，一定要以test开头  
    def testsum(self):  
        self.assertEqual(myclass.sum(1, 2), 2, 'test sum fail')  
          
          
    def testsub(self):  
        self.assertEqual(myclass.sub(2, 1), 1, 'test sub fail')     
          
          
if __name__ =='__main__':  
    unittest.main() 


# 这种方式执行单个测试文件时使用-v参数可以获得更多的测试结果信息。如：mytest.py -v  
