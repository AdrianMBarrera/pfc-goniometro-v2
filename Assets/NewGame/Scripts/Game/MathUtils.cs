using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MathUtils {
	
	public static float AngToPercent(float angle){
				
		float percent = 0f;
		
		percent = (angle * -100) / GameManager.instance.maximo;
		
		if (percent < 0)
			percent = 0;
		else if (percent > 100)
			percent = 100;
		
		return percent;
	}
		
}

