def print_turn(circle, current, turn, player)
  print "[#{turn}][#{player}] "

  circle.each do |c|
    if c == current
      print "(#{c}) "
    else
      print c.to_s + ' '
    end
  end

  puts
end


# 9 players; last marble is worth 25 points: high score is 32
lines = File.readlines('../i.txt')

lines.each do |line|
  arr = line.split(' ')
  players = Array.new(Integer(arr[0]),0)
  marbles = Integer(arr[6])
  score = 0

  print "Playing a game with #{players.length} players and max marble is #{marbles}"

  if line.include? 'high score'
    score = Integer(arr[11])
    print " the score should be #{score}"
  end

  puts

  # first 2 rounds are always the same
  marble = 0
  current = 0
  turn = 0
  circle = [0]
  #print_turn(circle, current, turn, '-')

  marble = 1
  current = 1
  turn = 1
  player = 0
  circle = [0, 1]
  #print_turn(circle, current, turn, player)

  marble = 2
  current = 2
  turn = 2
  player = 1
  last_current = 1

  while marble <= marbles

    last_current_index = circle.index(last_current)

    if marble % 23 == 0

      players[player] += marble

      special_index = last_current_index - 7
      # now the the 7th marble to the left from the current
      if special_index < 0
        special_index = last_current_index + circle.length - 7
      end

      players[player] += circle[special_index]
      circle.delete_at(special_index)

      current = circle[special_index]
      # print_turn(circle, current, turn, player)
      last_current = current
      marble += 1
    else
      desired_index = last_current_index + 2
      if desired_index > circle.length
        target_index = desired_index / circle.length
      else
        target_index = desired_index
      end


      if desired_index == circle.count
        circle = circle  + [marble]
      else
        circle_pre = circle[0..target_index-1]
        circle_after = circle[target_index..circle.length-1]
        circle = circle_pre + [marble] + circle_after
      end

      #print_turn(circle, marble, turn, player)
      last_current = marble
      marble += 1
      turn += 1


    end

    if player < players.length-1
      player += 1
    else
      player = 0
    end
  end

  sorted_players = players.sort.reverse

  high_score = sorted_players.first
  print "high score is: #{high_score} "
  if score > 0
    print "should be #{score}"
  end

  puts
end

