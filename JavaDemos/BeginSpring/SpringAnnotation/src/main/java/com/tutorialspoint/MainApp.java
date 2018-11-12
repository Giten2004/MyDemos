package com.tutorialspoint;


import org.springframework.context.support.AbstractApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

public class MainApp {
    public static void main(String[] args) {

        AbstractApplicationContext context = new ClassPathXmlApplicationContext("Beans.xml");

        // Let us raise a start event.
        context.start();

        Student student = (Student) context.getBean("student");
        System.out.println("Name : " + student.getName());
        System.out.println("Age : " + student.getAge());


        System.out.println("################# Auto Wired #################");
        TextEditor te = (TextEditor) context.getBean("textEditor");
        te.spellCheck();

        System.out.println("#################################");
        HelloWorld obj = (HelloWorld) context.getBean("helloWorld");
        obj.getMessage();
        context.registerShutdownHook();

        // Let us raise a stop event.
        context.stop();
    }
}