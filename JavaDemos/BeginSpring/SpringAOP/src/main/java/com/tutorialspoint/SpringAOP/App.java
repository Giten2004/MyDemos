package com.tutorialspoint.SpringAOP;

import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

/**
 * Hello world!
 */
public class App {
    public static void main(String[] args) {

        ApplicationContext context = new ClassPathXmlApplicationContext("application.xml");
        Say say = (Say) context.getBean("say");
        say.say();

        System.out.println("#############   another demo   #############");

        UserService userService = (UserService) context.getBean("userService");
        userService.getDemoUser();
    }
}
