using System;
using TechTalk.SpecFlow;
using Driver = Specs.Support.Driver_WPF;

namespace Specs.Steps
{
    [Binding]
    public class ColorPickerSteps
    {
        private Driver _driver;

        public ColorPickerSteps(Driver driver)
        {
            _driver = driver;
        }

        [Given]
        public void Given_that_the_cursor_is_over_the_current_color_box()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given]
        public void Given_that_the_color_picker_is_open()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When]
        public void When_I_left_click()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When]
        public void When_I_move_the_picker_handle()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_Color_picker_should_open()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then]
        public void Then_the_current_color_should_change()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
