package demo;

import java.io.IOException;

import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;

public class HelloFilter implements Filter {

	@Override
	public void init(FilterConfig arg0) throws ServletException {
		System.out.println("Filter 初始化");
	}

	@Override
	public void doFilter(ServletRequest req, ServletResponse res,
			FilterChain chain) throws IOException, ServletException {
		HttpServletRequest request = (HttpServletRequest)req;
		System.out.println("拦截 URI="+request.getRequestURI());
		chain.doFilter(req, res);
	}

	@Override
	public void destroy() {
		System.out.println("Filter 结束");
	}
}
