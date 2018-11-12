package com.mycompany.mvnbook.HelloMaven;

import static org.junit.Assert.assertEquals;
import org.junit.Test;

public class HelloWorldTest{
	@Test
	public void testsayHello(){
		HelloWorld helloWorld = new HelloWorld();
		String result = helloWorld.sayHello();
		
		assertEquals("Hello Maven", result);
	}
}
