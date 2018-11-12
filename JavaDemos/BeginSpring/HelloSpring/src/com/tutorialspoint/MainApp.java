package com.tutorialspoint;

import org.springframework.context.ApplicationContext;
import org.springframework.context.support.AbstractApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import org.springframework.beans.factory.InitializingBean;
import org.springframework.beans.factory.xml.XmlBeanFactory;
import org.springframework.core.io.ClassPathResource;

public class MainApp {
	public static void main(String[] args){
		//1) 使用企业容器
		//ApplicationContext context = new ClassPathXmlApplicationContext("Beans.xml");
		//HelloWorld obj = (HelloWorld)context.getBean("helloWorld");		
		//obj.getMessage();
		
		//2) 使用简单的容器
		/***
		XmlBeanFactory factory = new XmlBeanFactory(new ClassPathResource("Beans.xml"));
		HelloWorld obj = (HelloWorld)factory.getBean("helloWorld");		
		
		obj.getMessage();
		obj.setMessage("Default scorpe is singleton?");
		
		HelloWorld objB = (HelloWorld) factory.getBean("helloWorld");
	    objB.getMessage();
		
	    System.out.println("############ SingleTon ########################");
	    HelloWorld singleTonObj = (HelloWorld)factory.getBean("singletonHelloWorld");
	    singleTonObj.getMessage();
	    singleTonObj.setMessage("set value for SingleTon");
	    
	    HelloWorld singleTonObjB = (HelloWorld)factory.getBean("singletonHelloWorld");
	    singleTonObjB.getMessage();
***/
		
		//3)
		AbstractApplicationContext context = new ClassPathXmlApplicationContext("Beans.xml");
	    HelloWorld obj = (HelloWorld) context.getBean("helloWorld");
	    obj.getMessage();
	    context.registerShutdownHook();
	}
}
