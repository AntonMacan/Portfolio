package hr.fer.oprpp1.gui.layouts;

import java.awt.Color;
import java.awt.Component;
import java.awt.Container;
import java.awt.Dimension;
import java.awt.LayoutManager2;
import java.awt.Rectangle;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.swing.BorderFactory;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.SwingConstants;
import javax.swing.border.Border;

public class CalcLayout implements LayoutManager2 {
	private List<Double> labelWidth;
	private List<Double> labelHeight;
	private double space;
	private List<RCPosition> list;
	private List<Component> comps;
	
	
	public CalcLayout(double space) {
		this.space = space;
		list = new ArrayList<>();
		labelWidth = new ArrayList<>();
		labelHeight = new ArrayList<>();
		comps = new ArrayList<>();
	}
	
	public CalcLayout() {
		this.space = 0;
		list = new ArrayList<>();
		labelWidth = new ArrayList<>();
		labelHeight = new ArrayList<>();
		comps = new ArrayList<>();
	}
	
	@Override
	public void addLayoutComponent(String name, Component comp) {
		throw new UnsupportedOperationException();
	}

	@Override
	public void removeLayoutComponent(Component comp) {
		int d = 0;
		for(Component c : comps) {
			if(comp.equals(c)) {
				break;
			}
			d++;
		}
		comps.remove(d);
		list.remove(d);
	}

	@Override
	public Dimension preferredLayoutSize(Container parent) {
		double minW = 0,minH = 0;
		for(Component c : parent.getComponents()) {
			Dimension p = c.getPreferredSize();
			if(p != null) {
				if(p.width > minW) {
					minW = p.width;
				}
				if(p.height > minH) {
					minH = p.height;
				}
			}
		}
		minW = 7 * minW + 6 * space;
		minH = 5* minH + 4 * space;
		int imaxW =(int) Math.ceil(minW);
		int imaxH = (int) Math.ceil(minH);
		return new Dimension(imaxW,imaxH);
	}

	@Override
	public Dimension minimumLayoutSize(Container parent) {
		double minW = 0,minH = 0;
		for(Component c : parent.getComponents()) {
			Dimension p = c.getMinimumSize();
			int x = c.getX(), y = c.getY();
			if(p != null) {
				if(p.width > minW) {
					minW = p.width;
				}
				if(p.height > minH) {
					minH = p.height;
				}
			}
		}
		minW = 7 * minW + 6 * space;
		minH = 5* minH + 4 * space;
		int imaxW =(int) Math.ceil(minW);
		int imaxH = (int) Math.ceil(minH);
		return new Dimension(imaxW,imaxH);
	}

	@Override
	public void layoutContainer(Container parent) {
		System.out.println("LC");
		double parentHeight = parent.getHeight();
		double parentWidth = parent.getWidth();
		double prefH = parentHeight - 4*space; 
		int restH = (int) (prefH%5);
		prefH = Math.floor(prefH/5);
		labelWidth.clear();
		labelHeight.clear();
		switch(restH){
			case 0:{
				for(int i = 0; i < 5; i++) {
					labelHeight.add(prefH);
				}
				break;
			}
			case 1:{
				for(int i = 0; i < 5; i++) {
					if(i == 2) {
						labelHeight.add(prefH+1);	
					}else {
						labelHeight.add(prefH);
					}

				}
				break;
			}
			case 2:{
				for(int i = 0; i < 5; i++) {
					if(i == 1 || i == 3) {
						labelHeight.add(prefH+1);	
					}else {
						labelHeight.add(prefH);
					}

				}
				break;
			}
			case 3:{
				for(int i = 0; i < 5; i++) {
					if(i == 0 || i == 2 || i == 4) {
						labelHeight.add(prefH+1);	
					}else {
						labelHeight.add(prefH);
					}

				}
				break;
			}
			case 4:{
				for(int i = 0; i < 5; i++) {
					if(i != 3) {
						labelHeight.add(prefH+1);	
					}else {
						labelHeight.add(prefH);
					}

				}
				break;
			}
		}
		double prefW = parentWidth - 6*space; 
		int restW = (int) (prefW%5);
		prefW = Math.floor(prefW/7);
		switch(restW){
		case 0:{
			for(int i = 0; i < 7; i++) {
				labelWidth.add(prefW);
			}
			break;
		}
		case 1:{
			for(int i = 0; i < 7; i++) {
				if(i == 3) {
					labelWidth.add(prefW+1);	
				}else {
					labelWidth.add(prefW);
				}

			}
			break;
		}
		case 2:{
			for(int i = 0; i < 7; i++) {
				if(i == 0 || i == 6) {
					labelWidth.add(prefW+1);	
				}else {
					labelWidth.add(prefW);
				}

			}
			break;
		}
		case 3:{
			for(int i = 0; i < 7; i++) {
				if(i == 0 || i == 3 || i == 6) {
					labelWidth.add(prefW+1);	
				}else {
					labelWidth.add(prefW);
				}

			}
			break;
		}
		case 4:{
			for(int i = 0; i < 7; i++) {
				if(i != 1 || i != 3 || i!=5) {
					labelWidth.add(prefW+1);	
				}else {
					labelWidth.add(prefW);
				}

			}
			break;
		}
		case 5:{
			for(int i = 0; i < 7; i++) {
				if(i != 0 || i != 6) {
					labelWidth.add(prefW+1);	
				}else {
					labelWidth.add(prefW);
				}

			}
			break;
		}
		case 6:{
			for(int i = 0; i < 7; i++) {
				if(i != 3) {
					labelWidth.add(prefW+1);	
				}else {
					labelWidth.add(prefW);
				}

			}
			break;
		}
		}
		int i = 0;
		if(labelWidth.size() == 7 && labelHeight.size() == 5) {
		for(Component c : parent.getComponents()) {
			RCPosition rc = list.get(i);
			int x = 0;
			int y = 0;	
			int width = 0;
			int height = 0;
			int row = rc.row;
			int column = rc.column;
			if(row == 1 && column == 1) {
				for(int j = 0; j < 5; j++) {
					width+=labelWidth.get(j);
				}
				width += space*4;
				height += labelHeight.get(0);
			}else {
				for(int j = 0; j < column-1; j++) {
					x+=labelWidth.get(j);
				}
				x += space*(column-1);
				for(int j = 0; j < row-1; j++) {
					y+=labelHeight.get(j);
				}
				y += space*(row-1);
				width = labelWidth.get(column-1).intValue();
				height = labelHeight.get(row-1).intValue();
			}
			c.setBounds(x, y, width, height);
			i++;
			comps.set(i-1, c);
		}
		}
		
	}

	@Override
	public void addLayoutComponent(Component comp, Object constraints) {
		if(comp == null || constraints == null) {
			throw new NullPointerException();
		}
		int row = 0,column = 0;
		RCPosition r = null;
		if(constraints.getClass().equals(RCPosition.class)) {
			r = (RCPosition) constraints;
			row = r.row;
			column = r.column;
		}else if(constraints.getClass().equals(String.class)) {
			r = RCPosition.Parse((String)constraints);
			row = r.row;
			column = r.column;
		}else {
			throw new IllegalArgumentException();
		}
		for(RCPosition rc : list){;
			if(rc.row == row && rc.column == column) {
				throw new CalcLayoutException();
			}
		}
		comps.add(comp);
		list.add(r);
	}

	@Override
	public Dimension maximumLayoutSize(Container target) {
		double maxW = 0,maxH = 0;
		for(Component c : target.getComponents()) {
			Dimension p = c.getMaximumSize();
			if(p != null) {
				if(p.width > maxW) {
					maxW = p.width;
				}
				if(p.height > maxH) {
					maxH = p.height;
				}
			}
		}

		maxW = 7 * maxW + 6 * space;
		maxH = 5* maxH + 4 * space;

		int imaxW =(int) Math.ceil(maxW);
		int imaxH = (int) Math.ceil(maxH);
		return new Dimension(imaxW,imaxH);
	}

	@Override
	public float getLayoutAlignmentX(Container target) {
		return 0;
	}

	@Override
	public float getLayoutAlignmentY(Container target) {
		return 0;
	}

	@Override
	public void invalidateLayout(Container target) {
	}


}
