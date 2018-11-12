package com.tutorialspoint.SpringAOP;

import org.aspectj.lang.ProceedingJoinPoint;
import org.aspectj.lang.annotation.*;
import org.springframework.stereotype.Service;

/**
 * Created by admin on 2015/9/2.
 */
@Aspect
@Service
public class XmlAopDemoUserLog2 {

    // 配置切点 及要传的参数
    @Pointcut("execution(* com.ganji.demo.service.user.UserService.GetDemoUser(..)) && args(id)")
    public void pointCut(int id) {

    }

    // 配置连接点 方法开始执行时通知
    @Before("pointCut(id)")
    public void beforeLog(int id) {
        System.out.println("开始执行前置通知  日志记录:" + id);
    }

    //    方法执行完后通知
    @After("pointCut(id)")
    public void afterLog(int id) {
        System.out.println("开始执行后置通知 日志记录:" + id);
    }

    //    执行成功后通知
    @AfterReturning("pointCut(id)")
    public void afterReturningLog(int id) {
        System.out.println("方法成功执行后通知 日志记录:" + id);
    }

    //    抛出异常后通知
    @AfterThrowing("pointCut(id)")
    public void afterThrowingLog(int id) {
        System.out.println("方法抛出异常后执行通知 日志记录" + id);
    }

    //    环绕通知
    @Around("pointCut(id)")
    public Object aroundLog(ProceedingJoinPoint joinpoint, int id) {
        Object result = null;
        try {
            System.out.println("环绕通知开始 日志记录" + id);
            long start = System.currentTimeMillis();

            //有返回参数 则需返回值
            result = joinpoint.proceed();

            long end = System.currentTimeMillis();
            System.out.println("总共执行时长" + (end - start) + " 毫秒");
            System.out.println("环绕通知结束 日志记录");
        } catch (Throwable t) {
            System.out.println("出现错误");
        }
        return result;
    }
}

